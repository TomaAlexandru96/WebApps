using System;

public class Ability {
	public const int Mele = 0; 
	public const int ForkBomb = 1; 
	public const int DebugGun = 2; 
	public const int ElectricShock = 3;

	public int type;

	public Ability (int type) {
		this.type = type;
	}

	public float GetCooldown () {
		switch (type) {
		case Mele:
			return 0.5f;
		case ForkBomb:
			return 20f;
		case DebugGun: 
			return 1f;
		case ElectricShock:
			return 5f;
		default:
			return 0f;
		}
	}

	public override string ToString () {
		switch (type) {
		case Mele:
			return "Mele";
		case ForkBomb:
			return "Fork Bomb";
		case DebugGun: 
			return "Debug Gun";
		case ElectricShock:
			return "Electric Shock";
		default:
			return "No ability";
		}
	}
}

