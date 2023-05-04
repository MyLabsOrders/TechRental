namespace RentDesktop.Models.Security
{
    internal interface ICaptcha
    {
        string Text { get; }
        int Length { get; set; }

        void UpdateText();
    }
}