using System.Collections.Generic;

enum PlayerStates
{
    none,
    idle,
    run,
    gunEquip,
    gunFire
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.none] = new PlayerNoneState(_context, this);
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
        _states[PlayerStates.gunEquip] = new PlayerGunEquipState(_context, this);
        _states[PlayerStates.gunFire] = new PlayerGunFireState(_context, this);
    }

    public PlayerBaseState None()
    {
        return _states[PlayerStates.none];
    }
    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Run()
    {
        return _states[PlayerStates.run];
    }

    public PlayerBaseState GunEquip()
    {
        return _states[PlayerStates.gunEquip];
    }

    public PlayerBaseState GunFire()
    {
        return _states[PlayerStates.gunFire];
    }
}
