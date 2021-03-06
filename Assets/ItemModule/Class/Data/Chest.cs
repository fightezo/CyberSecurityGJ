namespace ItemModule.Class.Data
{
    public abstract class Chest : DefenderItem
    {
        private string _password;
        private string _restrictedCharacters;

        public string GetRestrictedCharacters()
        {
            return _restrictedCharacters;
        }

        public void SetRestrictedCharacters(string restrictedCharacters)
        {
            //^[0-9]+$ digits only
            //^[a-zA-Z][a-zA-Z0-9]*$ alpha numeric
        
            _restrictedCharacters = restrictedCharacters;
        }
        public string GetPassword()
        {
            return _password;
        }
        public void SetPassword(string password)
        {
            _password = password;
        }

        public abstract int GetPasswordLength();
    }
}
