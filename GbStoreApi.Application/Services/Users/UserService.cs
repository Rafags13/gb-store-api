using AutoMapper;
using GbStoreApi.Application.Exceptions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.Dto.Users.Dashboard;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Security.Claims;

namespace GbStoreApi.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        private readonly Lazy<IPurchaseService> _purchaseService;
        public UserService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context,
            IMapper mapper,
            Lazy<IPurchaseService> purchaseService
            )
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
            _purchaseService = purchaseService;
        }
        public ResponseDto<IEnumerable<DisplayUserDto>> GetAll()
        {
            var users = _unitOfWork.User.GetAll().Select(user => _mapper.Map<DisplayUserDto>(user)) ??
                Enumerable.Empty<DisplayUserDto>().AsQueryable();

            return new ResponseDto<IEnumerable<DisplayUserDto>>(users);
        }

        public ResponseDto<DisplayUserDto> GetById(int id)
        {

            var selectedUser =
                _unitOfWork.User.GetById(id);

            if (selectedUser is null)
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status404NotFound, "Nenhum usuário encontrado.");
            
            var userMapped = _mapper.Map<DisplayUserDto>(selectedUser);


            return new ResponseDto<DisplayUserDto>(userMapped);
        }

        public ResponseDto<User> GetByCredentials(SignInDto signInDto)
        {
            var currentUser = _unitOfWork.User.FindOne(x =>
                x.Email == signInDto.Email
            );

            if (currentUser is null)
                return new ResponseDto<User>(StatusCodes.Status404NotFound, "Não existe nenhum usuário com esse e-mail. Tente Novamente.");
            var passwordsMatch = BCrypt.Net.BCrypt.Verify(signInDto.Password, currentUser.Password);

            if (!passwordsMatch)
                return new ResponseDto<User>(StatusCodes.Status404NotFound, "A senha informada está incorreta. Tente Novamente.");
            
            return new ResponseDto<User>(currentUser);
        }

        public ResponseDto<DisplayUserDto> GetCurrentInformations()
        {
            var currentUserId = _context.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status401Unauthorized, "Usuário inválido");

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == int.Parse(currentUserId));
            if (currentUser is null)
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status404NotFound, "Não foi encontrado nenhum usuário com esse e-mail.");

            var displayUser = _mapper.Map<DisplayUserDto>(currentUser);

            return new ResponseDto<DisplayUserDto>(displayUser);
        }

        public ResponseDto<string?> GetUserRole()
        {
            try
            {
                var currentUserId = GetLoggedUserId();

                var currentUser = _unitOfWork.User.FindOne(x => x.Id == currentUserId);

                if (currentUser is null)
                    return new ResponseDto<string?>(StatusCodes.Status404NotFound, "Não foi possível encontrar o usuário.");

                return new ResponseDto<string?>(currentUser.TypeOfUser.ToString());
            }
            catch (UserNotValidException)
            {
                return new ResponseDto<string?>(null);
            }
        }

        public ResponseDto<bool> Update(UpdateUserDto updateUserDto)
        {
            var currentUser = _unitOfWork.User.GetAll().FirstOrDefault(x => x.Cpf == updateUserDto.Cpf);

            if (currentUser is null)
                return new ResponseDto<bool>(StatusCodes.Status404NotFound, "Não existe nenhum usuário com esse cpf.");

            _mapper.Map(updateUserDto, currentUser);

            _unitOfWork.User.Update(currentUser);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível salvar as alterações do usuário.");

            return new ResponseDto<bool>(true);
        }

        public ResponseDto<bool> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var currentUserResponse = GetCurrentInformations();

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == currentUserResponse.Value.Id);
            if (currentUser is null)
                return new ResponseDto<bool>(StatusCodes.Status404NotFound, "Não foi possível encontrar o usuário");

            var isSamePassword = BCrypt.Net.BCrypt.Verify(updatePasswordDto.OldPassword, currentUser.Password);
            if (!isSamePassword)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "A senha informada não é equivalente à antiga.");

            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);
            _unitOfWork.User.Update(currentUser);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status422UnprocessableEntity, "Não foi possível atualizar a senha.");

            return new ResponseDto<bool>(StatusCodes.Status200OK);
        }

        public int GetLoggedUserId()
        {
            var currentUserIdInClaims = _context.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(currentUserIdInClaims, out int currentLoggedUserId))
                throw new UserNotValidException("Não foi possível recuperar o usuário logado");

            return currentLoggedUserId;
        }

        private (int, int) GetNewAccountsDifferenceByMonthIndex(int monthIndex)
        {
            var newAccountsCount = _unitOfWork.User.Count(x => x.CreatedAt.Month == monthIndex);
            var previousAccountCount = _unitOfWork.User.Count(x => x.CreatedAt.Month == (monthIndex == 1 ? 12 : monthIndex - 1));

            return new(newAccountsCount, previousAccountCount);
        }

        public List<CardCountDto> GetCardsValueByMonthIndex(int monthIndex)
        {
            var (currentMonthCount, previousMonthCount) = _purchaseService.Value.GetDifferenceCountByMonthIndex(monthIndex);
            var (currentMonthSum, previousMonthSum) = _purchaseService.Value.GetDifferenceSumByMonthIndex(monthIndex);
            var (currentAccountCount, previousAccountCount) = GetNewAccountsDifferenceByMonthIndex(monthIndex);
            var (currentMultipleProductsCount, previousMultipleProductsCount) = _purchaseService.Value.GetMultipleProductsDifferenceCountByMonthIndex(monthIndex);

            return new List<CardCountDto>
            {
                new()
                {
                    Label = "Total de Vendas",
                    Value = currentMonthCount,
                    PercentageOfGainOrLose = currentMonthCount == 0 && previousMonthCount == 0 ? 0 :
                    ((currentMonthCount * 100)/(previousMonthCount == 0 ? 1 : previousMonthCount)) - 100
                },
                new()
                {
                    Label = "Ganhos",
                    Value = currentMonthSum,
                    PercentageOfGainOrLose =
                        currentMonthSum == 0 && previousMonthCount == 0 ? 0 :
                        (float)((currentMonthSum * 100)/(previousMonthSum == 0 ? 1 : previousMonthSum)) - 100
                },
                new()
                {
                    Label = "Novas Contas",
                    Value = currentAccountCount,
                    PercentageOfGainOrLose =
                        currentAccountCount == 0 && previousAccountCount == 0 ? 0 :
                        ((currentAccountCount * 100)/(previousAccountCount == 0 ? 1 : previousAccountCount)) - 100
                },
                new()
                {
                    Label = "Múltiplos Produtos Em Compras",
                    Value = currentMultipleProductsCount,
                    PercentageOfGainOrLose =
                        currentMultipleProductsCount == 0 && previousMultipleProductsCount == 0 ? 0 :
                        ((currentMultipleProductsCount * 100)/(previousMultipleProductsCount == 0 ? 1 : previousMultipleProductsCount)) - 100
                }
            };
        }

        public List<CellValueLineChart> GetLineChartDataByLastSixMonths(int monthIndex)
        {
            monthIndex = monthIndex == 1 ? 12 : monthIndex;

            var currentMonthIndexes = new List<int>();

            while(currentMonthIndexes.Count != 6)
            {
                currentMonthIndexes.Add(monthIndex);
                monthIndex--;
                if (monthIndex == 0) monthIndex = 12;
            }

            var purchasesFromLastSixMonths = _unitOfWork.Purchase
                .GetAll()
                .Where(x => currentMonthIndexes.Contains(x.OrderDate.Month))
                .Select(x => new
                {
                    MonthIndex = x.OrderDate.Month,
                    FullMonthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.OrderDate.ToString("MMMM")),
                    AbbreviatedMonthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.OrderDate.ToString("MMM")),
                    x.FinalPrice,
                })
                .GroupBy(x => x.MonthIndex)
                .Select(x => new CellValueLineChart
                {
                    MonthIndex = x.Key,
                    AbbreviatedMonthName = x.First().AbbreviatedMonthName,
                    FullMonthName = x.First().FullMonthName,
                    Earns = x.Sum(x => x.FinalPrice)
                }).ToList();

            List<CellValueLineChart> lineCharts = new();

            lineCharts.AddRange(purchasesFromLastSixMonths);

            currentMonthIndexes.ForEach(monthIndex =>
            {
                if(!lineCharts.Any(x => x.MonthIndex.Equals(monthIndex)))
                {
                    lineCharts.Add(new CellValueLineChart
                    {
                        MonthIndex = monthIndex,
                        AbbreviatedMonthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthIndex)),
                        FullMonthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthIndex)),
                        Earns = 0
                    });
                }
            });

            return lineCharts.OrderBy(x => x.MonthIndex).ToList();
        }

        public ResponseDto<DashboardSummaryDto> GetDashboardSummary(int monthIndex)
        {
            var cards = GetCardsValueByMonthIndex(monthIndex);
            var lineChart = GetLineChartDataByLastSixMonths(monthIndex);

            var dashboardData = new DashboardSummaryDto
            {
                SystemAmountSpecificationCard = cards,
                SellingProgressionLastSixMonths = lineChart,
            };

            return new(dashboardData);
        }

    }
}
