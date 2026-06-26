namespace task04;

public class Fighter : ISpaceShip
{
    public int Speed => 100;
    public int FirePower => 50;
    public void MoveForward() { }
    public void Rotate(int angle) { }
    public void Fire() { }
}