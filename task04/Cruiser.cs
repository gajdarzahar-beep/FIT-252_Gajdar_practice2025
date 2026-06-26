namespace task04;

public class Cruiser : ISpaceShip
{
    public int Speed => 50;
    public int FirePower => 100;
    public void MoveForward() { }
    public void Rotate(int angle) { }
    public void Fire() { }
}