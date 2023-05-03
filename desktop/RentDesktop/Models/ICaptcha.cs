namespace RentDesktop.Models
{
    public interface ICaptcha
    {
        string Text { get; }
        int Length { get; set; }

        void UpdateText();
    }
}