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

        public class PortfolioSummary
        {
            public string FirmID;
            public string ID;
            public string Name;
            public string FirstName;
            public string Surname;
            public string Language;
            public string Currency;
            public List<int> NodeID;
            public List<string> NodeName;
            public string Status;
            public string ClientType;
            public PortfolioPosition[] Positions;
            public PortfolioTransaction[] CompleteTransactions;
            public SubAccount[] Accounts;
            public decimal BookValue;
            public decimal NonTransferBookValue;
            public decimal TransferBookValue;
            public decimal OpeningValue;
            public decimal CurrentValue;
            public decimal UninvestedCash;
            public decimal ClosingCashValue;
            public decimal Growth;
            public decimal GrowthPercent;
            public decimal AdjustedGrowth;
            public decimal AdjustedGrowthPercent;
            public decimal TransferValue;
            public decimal UncrystallisedValue;
            public Product[] Products;
        }

        public class PortfolioPosition
        {
            public string PositionType;
            public string ISIN;
            public string AssetID;
            public string AssetName;
            public decimal Quantity;
            public decimal BookValue;
            public decimal TransferBookValue;
            public decimal NonTransferBookValue;
            public string Currency;
            public decimal CurrentValue;
            public decimal OpeningValue;
            public decimal Growth;
            public decimal GrowthPercent;
            public decimal AdjustedGrowth;
            public decimal AdjustedGrowthPercent;
            public decimal CurrentPrice;
            public string CurrentPriceDate;
            public decimal MinimumTransferUnit;
            public decimal Allocation;
        }

        public class PortfolioTransaction
        {
            public string ID;
        }

        public class SubAccount
        {
            public string ID;
            public string Name;
            public string AccountType;
            public string Currency;
            public string WrapperType;
            public int NodeID;
            public string assetAllocationID;
            public string AssetAllocationName;
            public string Status;
            public bool RecurringPayment;
            public WrapperDetail WrapperDetail;
            public decimal CurrentValue;
            public decimal OpeningValue;
            public decimal OpeningStockValue;
            public decimal OpeningCashValue;
            public decimal BookValue;
            public decimal TransferBookValue;
            public decimal NonTransferBookValue;
            public decimal Growth;
            public decimal AdjustedGrowth;
            public decimal ClosingCashValue;
            public decimal UninvestedCash;
            public decimal ClosingStockValue;
            public decimal GrowthPercent;
            public decimal AdjustedGrowthPercent;
            public decimal TransferValue;
            public decimal Allocation;
        }

        public class WrapperDetail
        {
            public string WrapperType;
            public bool Discretionary;
            public bool Advised;
            public bool Trust;
            public string ClientProductId;
            public string SchemeProductId;
            public string assetAllocationID;
            public string AssetAllocationName;
            public string ProductStatus;
        }

        public class Product
        {
            public string WrapperType;
            public decimal RemainingSubscriptionAmount;
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
