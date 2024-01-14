using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1_SECCL_API.Classes
{
    public class Portfolio
    {
        public class PorfolioAccount
        {
            public string ID;
            public string FirmID;
            public string Name;
            public string Status;
            public string Currency;
            public decimal CurrentValue;
            public int Accounts;
            public decimal UninvestedCash;
            public decimal Growth;
            public decimal GrowthPercent;
            public decimal AdjustedGrowth;
            public decimal AdjustedGrowthPercent;
        }

        public struct PortfolioObject
        {
            public PortfolioData[] Data;
            public PortfolioMeta Meta;
        }

        public struct PortfolioData
        {
            public string ID;
            public string FirmID;
            public string Name;
            public string Status;
            public string Currency;
            public decimal CurrentValue;
            public int Accounts;
            public decimal UninvestedCash;
            public decimal Growth;
            public decimal GrowthPercent;
            public decimal AdjustedGrowth;
            public decimal AdjustedGrowthPercent;
        }

        public struct PortfolioMeta
        {
            public int count;
        }
    }

    
}
