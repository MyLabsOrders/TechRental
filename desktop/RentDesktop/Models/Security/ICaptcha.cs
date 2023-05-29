namespace RentDesktop.Models.Security
{
    public interface ICaptcha
    {
        string Text { get; }
        int Length { get; set; }

        void UpdateText();
    }
}