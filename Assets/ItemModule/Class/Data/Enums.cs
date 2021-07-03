namespace ItemModule.Class.Data
{
    public enum ItemType
    {
        None = 0,
        // Securing Tools
        NumericChest = 1,
        AlphanumericChest = 2,
        NormalChest = 3,
        TwoStepAuthChest = 4,
        // Action Tools
        AntiVirus = 101,
        Firewall = 102,
        // Others
        OneTimePassword = 103,
        
        // Hacking Tools
        BruteForce = 1001,
        Phishing = 1002,
        Spyware = 1003,
        // Action Tools
        Trojan = 1101,
        Ransomware = 1102,
        Stalker = 1103,
        
    }

    public enum ItemState
    {
        World,
        Player,
        Active,
    }

    public enum ItemCategory
    {
        Placement = 1,
        Action = 2,
    }

    public enum ItemOwner
    {
        None = 0,
        Defender = 1,
        Hacker = 2,
    }
}