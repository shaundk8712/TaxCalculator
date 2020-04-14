using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Application.Calculate
{
    public class Calculate
    {
        public double AnnualIncomeTax(double incomeAmount, string postalCode)
        {
            string taxType = TaxType(postalCode);
            double taxAmount = 0;

            switch (taxType)
            {
                case "Progressive":
                    taxAmount = ProgressiveTax(incomeAmount);
                    break;
                case "Flat Value":
                    taxAmount = FlatValueTax(incomeAmount);
                    break;
                case "Flat rate":
                    taxAmount = FlatRateTax(incomeAmount);
                    break;
                default:
                    break;
            }

            return taxAmount;
        }

        public static double ProgressiveTax(double incomeAmount)
        {
            List<TaxRange> taxRanges = new List<TaxRange>();

            taxRanges.AddRange(new TaxRange[]{
                new TaxRange{TaxId=1,MinRangeAmount=0,MaxRangeAmount=8350,TaxRate=10,TaxRateAmount=8350*10/100},
                new TaxRange{TaxId=2,MinRangeAmount=8351,MaxRangeAmount=33950,TaxRate=15,TaxRateAmount=33950*15/100},
                new TaxRange{TaxId=3,MinRangeAmount=33951,MaxRangeAmount=82250 ,TaxRate=25,TaxRateAmount=82250*25/100},
                new TaxRange{TaxId=4,MinRangeAmount=82251,MaxRangeAmount=171550 ,TaxRate=28,TaxRateAmount=171550*28/100},
                new TaxRange{TaxId=4,MinRangeAmount=171551,MaxRangeAmount=372950  ,TaxRate=33,TaxRateAmount=372950*33/100},
                new TaxRange{TaxId=4,MinRangeAmount=372951,MaxRangeAmount=double.MaxValue,TaxRate=35,TaxRateAmount=double.MaxValue*35/100},
              });

            return taxRanges.Where(x => x.MinRangeAmount < incomeAmount).Select(x => new
            {
                Tax =
                (
                    (x.MinRangeAmount <= incomeAmount && incomeAmount <= x.MaxRangeAmount) ?
                    Math.Round(((incomeAmount - x.MinRangeAmount) * x.TaxRate / 100), 0) :
                    Math.Round((x.MaxRangeAmount - x.MinRangeAmount) * x.TaxRate / 100, 0)
                )
            }).Sum(x => x.Tax);
        }

        public static double FlatValueTax(double incomeAmount)
        {
            return incomeAmount < 200000 ? (incomeAmount * 5 / 100) : 10000;
        }

        public static double FlatRateTax(double incomeAmount)
        {
            return (incomeAmount * 17.5 / 100);
        }

        public string TaxType(string postalCode)
        {
            string taxType = "Progressive";

            switch (postalCode)
            {
                case "7441":
                    taxType = "Progressive";
                    break;
                case "A100":
                    taxType = "Flat Value";
                    break;
                case "7000":
                    taxType = "Flat rate";
                    break;
                case "1000":
                    taxType = "Progressive";
                    break;
                default:
                    break;
            }

            return taxType;
        }
    }

    public class TaxRange
    {
        public int TaxId { get; set; }
        public double MinRangeAmount { get; set; }
        public double MaxRangeAmount { get; set; }
        public int TaxRate { get; set; }
        public double TaxRateAmount { get; set; }
    }
}
