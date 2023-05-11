namespace BlazorApp1.Client.State
{
    public class UserState
    {
        private string? displayName;

        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
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
