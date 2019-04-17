using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position { Gauche, Droite }

public enum PlayerType { IA, Joueur}

public enum FighterStates
{
    Idle, Walk, Walk_Back, Jump, Hit_Fall, PowerPerso, DoublePunch, DoubleKick,Load,
    KickL, KickR, PunchL, PunchR, Hit, Hit_Defend, Defend, Celebrate, Dead, Run, Teleportation, Combo, DeadEnd, PunchUpperCut,JumpAttack,Hit_Power,JumpFall, None
}


