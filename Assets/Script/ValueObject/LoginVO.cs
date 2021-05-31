public enum TankCategory
{
    Red = 0,
    Blue = 1
}

public class LoginVO
{
    public string type;
    public TankCategory tank;
    public string name;


    public LoginVO()
    {

    }

    public LoginVO(string type, TankCategory tank, string name)
    {
        this.type = type;
        this.tank = tank;
        this.name = name;
    }
}
