namespace BlazorApp1.Client.State
{
    public class UserState
    {
        private string? email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
