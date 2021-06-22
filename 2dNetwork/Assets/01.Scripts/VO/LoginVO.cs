
public enum TankCategory
{
    Red = 0,
    Blue = 1
}

public class LoginVO 
{
    public TankCategory tank;
    public string name;

    public LoginVO()
    {

    }

    public LoginVO(TankCategory tank, string name)
    {
        this.tank = tank;
        this.name = name;
    }
}
