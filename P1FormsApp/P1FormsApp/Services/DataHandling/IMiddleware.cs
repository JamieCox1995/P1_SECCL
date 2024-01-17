using P1_SECCL_API.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataHandling
{
    public interface IMiddleware
    {
        public Authentication.AuthenticationToken AuthenticateSession(string _FirmID, string _UserID, string _Password);

        public List<Portfolio.PorfolioAccount> GetPortfoliosForFirm(string _FirmID, string _AuthenticationToken);

        public Portfolio.PortfolioSummary GetPortfoliosSummary(string _FirmID, string _ID, string _AuthenticationToken);

        public decimal GetFirmAverageCashValue(List<Portfolio.PorfolioAccount> _Portfolios);

        public decimal GetTotalPositionValue(Portfolio.PortfolioSummary _PortfolioSummary);

        public decimal GetTotalAccountsValue(Portfolio.PortfolioSummary _PortfolioSummary);
    }
}
