using System;

namespace AppleGooglePay.Mobile.Payment
{
    public interface IPaymentService
    {
        event EventHandler CanMakePaymentsUpdated;
        event EventHandler<string> AuthorizationComplete;
        bool CanMakePayments { get; }

        void AuthorizePayment(decimal total);
    }
}
