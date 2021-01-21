using System;

namespace RentalAgreement.Model
{
    public class AgreementData
    {

        public DateTime Date { get; set; }
        public string AptAddress { get; set; }
        public string AptFeaturesBath { get; set; }
        public string AptFeatures { get; set; }
        public string AptFurnishing { get; set; }
        public string Appliances { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string CancelNoticePeriodDays { get; set; }
        public string ContinueNoticePeriondDays { get; set; }
        public double MonthPayment { get; set; }
        public double NonsufficientFundsFee { get; set; }
        public double LateFee { get; set; }
        public double SecurityDeposit { get; set; }
        public double RightToBuyPrice { get; set; }
        public double RightToBuyDeposit { get; set; }
        public string AbandonmentPeriod { get; set; }
        public string ParkingSpace { get; set; }
        public string ParkingSpaceDescription { get; set; }
        public int TerminationPeriod { get; set; }
        public double TerminationFee { get; set; }
        public string PetsAllowed { get; set; }
        public double PetsFee { get; set; }
        public string ArmedForcesNotice { get; set; }
        public string LawsState { get; set; }


        public override string ToString() 
        {
            return  "AgreementData{" +  
                    "Date=" + Date +
                    ", AptAddress=" + AptAddress +
                    ", AptFeaturesBath=" + AptFeaturesBath +
                    ", AptFeatures=" + AptFeatures +
                    ", AptFurnishing=" + AptFurnishing +
                    ", Appliances=" + Appliances +
                    ", DateBegin=" + DateBegin +
                    ", DateEnd=" + DateEnd +
                    ", CancelNoticePeriodDays=" + CancelNoticePeriodDays +
                    ", ContinueNoticePeriondDays=" + ContinueNoticePeriondDays +
                    ", MonthPayment=" + MonthPayment +
                    ", NonsufficientFundsFee=" + NonsufficientFundsFee +
                    ", LateFee=" + LateFee +
                    ", SecurityDeposit=" + SecurityDeposit +
                    ", RightToBuyPrice=" + RightToBuyPrice +
                    ", RightToBuyDeposit=" + RightToBuyDeposit +
                    ", AbandonmentPeriod=" + AbandonmentPeriod +
                    ", ParkingSpace=" + ParkingSpace +
                    ", ParkingSpaceDescription=" + ParkingSpaceDescription +
                    ", TerminationPeriod=" + TerminationPeriod +
                    ", TerminationFee=" + TerminationFee +
                    ", PetsAllowed=" + PetsAllowed +
                    ", PetsFee=" + PetsFee +
                    ", ArmedForcesNotice=" + ArmedForcesNotice +
                    ", LawsState=" + LawsState +
                    "}";
        }
    }
}