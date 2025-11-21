# IMG420-Assignment-6

This project implements an advanced enemy AI system using Behavior Trees in Godot 4.4 with C#. The enemy exhibits complex behaviors including patrolling, chasing, attacking, fleeing, and summoning allies based on various conditions and priority levels.

## Features Implemented

### Core AI Behaviors
- **Patrolling**: Enemies patrol between waypoints when idle
- **Player Detection**: Enemies detect player within range and engage
- **Chasing**: Enemies pursue the player when detected
- **Attacking**: Enemies deal damage when in attack range
- **Contact Damage**: Enemies deal damage on physical contact with player
- **Fleeing**: Enemies flee when health drops below 20%
- **Ally Summoning**: Enemies call for help when health drops below 50%

### Behavior Tree Implementation
Complete behavior tree with proper priority system:
1. **Emergency Flee** (Highest Priority) - Health < 20%
2. **Call for Help** - Health < 50% and allies available
3. **Combat** - Player detected, attack or chase
4. **Patrol** (Default) - Patrol waypoints when idle

### Technical Components
- **BTNode Base Class**: Abstract base for all behavior tree nodes
- **Composite Nodes**: BTSelector and BTSequence for control flow
- **Condition Nodes**: Health checks, range checks, cooldown checks
- **Action Nodes**: Movement, attacking, fleeing, summoning behaviors
- **Health System**: Visual health bars with color-coded feedback
- **Animation System**: State-based animations (idle, walk, attack)

## Setup Instructions

### Prerequisites
- Godot 4.4 or later
- .NET SDK 8.0 or later
- C# support enabled in Godot

### Installation Steps

1. **Clone or Extract the Project**
   ```bash
   git clone (https://github.com/acc668/IMG420-Assignment-6)
   cd IMG420-Assignment-6
   ```

2. **Open in Godot**
   - Launch Godot 4.4
   - Click "Import"
   - Navigate to the project folder
   - Select `project.godot`
   - Click "Import & Edit"

3. **Build C# Project**
   - In Godot, go to **Project → Tools → C# → Build Project**
   - Wait for "Build succeeded" message
   - If build fails, check that .NET SDK is properly installed

4. **Configure Collision Layers** (Critical for proper gameplay)
   - **Player**: Layer 1, Mask 3
   - **Enemy**: Layer 2, Mask 3
   - **Ally**: Layer 2, Mask 3
   - **Projectile (Area2D)**: Layer 3, Mask 2
   - **DetectionArea (Area2D)**: Layer 3, Mask 1

   This setup prevents enemies/allies from getting stuck on the player while maintaining proper hit detection.

5. **Verify Scene Setup**
   - Open `Main.tscn`
   - Ensure at least 2 Enemy instances are present
   - Verify each Enemy has patrol points configured
   - Confirm Ally scene is linked in Enemy's Inspector

### Running the Game

1. Press **F5** or click the Play button
2. Use **WASD** or **Arrow Keys** to move
3. Press **Space** or **Left Mouse Button** to attack
4. Observe enemy AI behaviors as you interact

## Controls

| Action | Key Binding |
|--------|-------------|
| Move Up | W / Up Arrow |
| Move Down | S / Down Arrow |
| Move Left | A / Left Arrow |
| Move Right | D / Right Arrow |
| Attack | Space / Left Mouse Button |

## Testing Report

### AI State Testing

#### 1. Patrol Behavior (Default State)
**Trigger**: Enemy is idle and has patrol points configured  
**Expected Behavior**:
- Enemy moves between patrol waypoints
- Plays "walk" animation while moving
- Shows "Patrolling" state label
- Continues until player is detected

**Test Results**: Working as expected
- Enemies smoothly patrol between configured waypoints
- Animation transitions correctly
- Returns to patrol after losing sight of player

#### 2. Player Detection & Chase
**Trigger**: Player enters detection range (200 units)  
**Expected Behavior**:
- Enemy stops patrolling
- Moves toward player position
- Plays "walk" animation
- Shows "Chasing" state label
- Maintains stopping distance to avoid getting stuck

**Test Results**: Working as expected
- Detection range works reliably
- Chase behavior is smooth and responsive
- Stopping distance prevents collision issues

#### 3. Attack Behavior
**Trigger**: Player within attack range (50 units) and attack cooldown ready  
**Expected Behavior**:
- Enemy stops moving
- Deals damage to player
- Respects attack cooldown (1 second)
- Shows "Attacking" state label

**Test Results**: Working as expected
- Attack timing is balanced
- Cooldown prevents spam damage
- Transitions smoothly between chase and attack

#### 4. Contact Damage
**Trigger**: Enemy's DetectionArea overlaps with Player  
**Expected Behavior**:
- Deals 10 damage per second on contact
- Has separate cooldown from regular attacks
- Works while chasing or fleeing

**Test Results**: Working as expected
- Contact damage encourages player to maintain distance
- Adds additional challenge to combat
- Cooldown prevents instant death

#### 5. Flee Behavior (Critical Health)
**Trigger**: Enemy health drops below 20%  
**Expected Behavior**:
- Highest priority - overrides all other behaviors
- Enemy runs away from player
- Moves at increased speed (150 units/s)
- Shows "Fleeing" state label
- Continues until reaching safe distance (300 units)

**Test Results**: Working as expected
- Priority system works correctly - flee takes precedence
- Enemy successfully creates distance
- Returns to other behaviors when safe

#### 6. Ally Summoning
**Trigger**: Enemy health below 50%, allies available, summon cooldown ready  
**Expected Behavior**:
- Enemy summons an ally at random position nearby
- Ally chases player immediately
- Maximum 2 allies per enemy
- Summon cooldown: 5 seconds
- Shows "Summoning" state label

**Test Results**: Working as expected
- Allies spawn correctly around enemy
- Allies properly chase and damage player
- Cooldown prevents spam summoning
- Max ally limit enforced

#### 7. Ally Behavior
**Trigger**: Ally is spawned  
**Expected Behavior**:
- Immediately chases player
- Deals damage on contact
- Has own health bar
- Can be killed by player projectiles
- Notifies parent enemy when destroyed

**Test Results**: Working as expected
- Allies provide meaningful challenge
- Can be destroyed by player
- Proper cleanup when defeated

## Project Structure

```
EnemyAI/
├── Scenes/
│   ├── Main.tscn              # Main game scene with TileMap and enemies
│   ├── Player.tscn            # Player character
│   ├── Enemy.tscn             # Enemy with behavior tree
│   ├── Ally.tscn              # Summoned ally
│   └── Projectile.tscn        # Player's projectile attack
├── Scripts/
│   ├── Player.cs              # Player movement and combat
│   ├── Enemy.cs               # Enemy controller with AI
│   ├── Ally.cs                # Ally AI (simplified enemy)
│   ├── Projectile.cs          # Projectile physics and collision
│   ├── HealthBar.cs           # Visual health display
│   └── BehaviorTree/
│       ├── BTNode.cs          # Base class for all BT nodes
│       ├── Composites/
│       │   ├── BTSelector.cs  # OR logic - tries children until success
│       │   └── BTSequence.cs  # AND logic - all must succeed
│       ├── Conditions/
│       │   ├── BTIsHealthCritical.cs    # Health < 20%
│       │   ├── BTIsHealthLow.cs         # Health < 50%
│       │   ├── BTAreAlliesAvailable.cs  # Can summon more allies?
│       │   ├── BTIsPlayerInRange.cs     # Distance checks
│       │   └── BTCanAttack.cs           # Cooldown ready?
│       └── Actions/
│           ├── BTFlee.cs          # Run away from player
│           ├── BTSummonAlly.cs    # Spawn ally
│           ├── BTAttack.cs        # Deal damage to player
│           ├── BTChasePlayer.cs   # Move toward player
│           └── BTPatrol.cs        # Move between waypoints
└── Assets/
    └── (sprites, animations, etc.)
```

## Enhancements Implemented

### 1. Visual Indicators
- **State Labels**: Real-time display of current AI state above each enemy
  - Shows: "Patrolling", "Chasing", "Attacking", "Fleeing", "Summoning"
  - Helps with debugging and understanding AI decisions
  
- **Color-Coded Health Bars**: Dynamic color based on health percentage
  - Green (>50%): Healthy
  - Yellow (20-50%): Wounded
  - Red (<20%): Critical
  - Provides immediate visual feedback on enemy status

- **Animation System**: State-based sprite animations
  - Idle: When not moving
  - Walk: During movement (patrol, chase, flee)
  - Attack: When attacking player
  - Smooth transitions between states
  - Sprite flipping based on movement direction

### 2. Advanced Combat System
- **Contact Damage**: Enemies deal damage on physical contact
  - Adds tactical depth - players must maintain distance
  - Separate cooldown from regular attacks
  - Works in all AI states
  
- **Projectile-Based Player Attack**
  - Player throws projectiles instead of melee
  - Projectiles can hit enemies and allies
  - Encourages positioning and aiming
  - Adds skill-based gameplay

### 3. Collision System Optimization
- **Layer-Based Physics**: Prevents enemies from getting stuck
  - Enemies and player don't physically collide
  - All gameplay mechanics (damage, detection) still work
  - Smooth movement without physics issues
  
- **Stopping Distance**: Enemies maintain distance when chasing
  - Prevents overlap and stacking
  - Creates better combat flow
  - Maintains visual clarity

### 4. Balanced Gameplay
**Tuning Parameters and Reasoning:**

| Parameter | Value | Reasoning |
|-----------|-------|-----------|
| Enemy MaxHealth | 100 | Allows for ~7 player hits, giving time to see all AI states |
| Enemy Chase Speed | 100 | Slightly slower than player (200), allows tactical retreat |
| Enemy Flee Speed | 150 | Faster than chase, ensures successful escape |
| Contact Damage | 10 | Punishes poor positioning without instant death |
| Contact Cooldown | 1s | Prevents unfair rapid damage, allows counterplay |
| Attack Damage | 10 | Same as contact, consistent threat level |
| Attack Cooldown | 1s | Balanced pacing, not overwhelming |
| Summon Cooldown | 5s | Prevents ally spam, makes each summon meaningful |
| Max Allies | 2 | Creates challenge without overwhelming player |
| Ally Health | 50 | Easier to kill than main enemy, but still threatening |
| Detection Range | 200 | Large enough to engage, small enough for stealth |
| Attack Range | 50 | Close enough to feel dangerous, allows dodging |
| Safe Distance (Flee) | 300 | Ensures enemy fully escapes before returning |
| Critical Health | 20% | Last-resort behavior, creates dramatic moments |
| Low Health | 50% | Early warning, gives enemy tactical options |

**Balance Philosophy**:
- Player should feel challenged but not overwhelmed
- Each AI state should be clearly observable
- Combat should reward skillful play (dodging, aiming)
- Enemy behaviors should feel intelligent and reactive
- Difficulty scales with multiple enemies

## Technical Implementation Details

### Behavior Tree Architecture

The behavior tree uses a priority-based system implemented through nested Selectors and Sequences:

```
Root (BTSelector) - Tries each child until one succeeds
├── Emergency Flee (BTSequence) - All conditions must pass
│   ├── IsHealthCritical? < 20%
│   └── Flee Action
├── Call for Help (BTSequence)
│   ├── IsHealthLow? < 50%
│   ├── AreAlliesAvailable?
│   └── SummonAlly Action
├── Combat (BTSequence)
│   ├── IsPlayerDetected?
│   └── CombatOptions (BTSelector)
│       ├── AttackSequence
│       └── ChaseSequence
└── Patrol (BTSequence) - Default behavior
    └── Patrol Action
```

**Key Design Decisions**:
- **Selector at root**: Implements priority - higher branches are checked first
- **Sequences for behaviors**: Ensures all conditions are met before action
- **Nested selector in combat**: Allows choosing between attack and chase
- **Modular nodes**: Each node is reusable and independently testable

### Animation State Management

Animations are managed through velocity-based state detection:
- Velocity > 10 units/s → Walk animation
- Velocity ≤ 10 units/s → Idle animation
- On attack trigger → Attack animation (timed override)

This approach:
- Automatically responds to movement
- Doesn't require explicit state management
- Handles edge cases gracefully
- Prevents animation stuttering

### Collision Layer System

Uses Godot's built-in physics layers for clean separation:
- **Layer 1**: Player body (can't collide with Layer 2)
- **Layer 2**: Enemy/Ally bodies (can't collide with Layer 1)
- **Layer 3**: Detection zones (Area2D nodes)

Benefits:
- Prevents physics-based stuck situations
- Maintains all gameplay interactions
- Clean and maintainable
- Easily extensible for new entity types

## Known Issues and Limitations

### Current Limitations

1. **Pathfinding**
   - Enemies use direct line movement, not A* pathfinding
   - Can get stuck on obstacles in complex environments
   - **Mitigation**: Keep level design relatively open, use TileMap walls strategically
   - **Future**: Implement NavigationAgent2D for proper pathfinding

2. **Ally Coordination**
   - Allies act independently, no group tactics
   - Multiple allies may stack on same position
   - **Mitigation**: Random spawn positions provide some spread
   - **Future**: Implement formation system or repulsion forces

3. **Line of Sight**
   - Enemies can detect player through walls
   - No occlusion checking for detection
   - **Mitigation**: Current detection range is balanced for this
   - **Future**: Add raycast-based line-of-sight checks

4. **Attack Animation Timing**
   - Attack animation plays but doesn't lock enemy in place
   - Could move during attack animation
   - **Mitigation**: Attack only triggers when in range and stopped
   - **Future**: Add animation lock system

5. **TileSet Warnings**
   - Console shows TileSet atlas warnings on startup
   - Does not affect gameplay
   - **Cause**: TileSet configuration includes unused tile coordinates
   - **Fix**: Clean up TileSet in editor to remove invalid tiles

### Performance Considerations

- **Good Performance**: Currently handles 5-10 enemies smoothly
- **Scaling**: Each behavior tree ticks every frame per enemy
- **Optimization**: Could implement tick throttling for distant enemies
- **Memory**: Minimal - all scripts are lightweight

### Edge Cases Handled

**Player out of bounds**: Enemies continue patrol or idle  
**No patrol points**: Enemies remain stationary until player detected  
**Ally scene not configured**: Warning logged, no crash  
**Multiple enemies summoning**: Each tracks own ally count  
**Rapid damage**: Cooldown systems prevent instant-death scenarios  

## Testing Checklist

- [x] Enemy patrols when idle
- [x] Enemy detects and chases player
- [x] Enemy attacks when in range
- [x] Enemy flees when health critical
- [x] Enemy summons allies when health low
- [x] Allies chase and attack player
- [x] Player can damage enemies with projectiles
- [x] Contact damage works correctly
- [x] Health bars update and change color
- [x] Animations play correctly for all states
- [x] State labels display current behavior
- [x] Multiple enemies work simultaneously
- [x] Collision layers prevent stuck enemies
- [x] Cooldowns function properly
- [x] Death and cleanup work correctly

## Build Information

- **Engine**: Godot 4.4
- **Language**: C# (.NET 8.0)
- **Platform**: Windows/Linux/Mac (cross-platform)
- **Resolution**: 1920x1080 (scalable)
- **Target FPS**: 60

## Credits

**Assets**:
- Enemy sprite: [Source if applicable]
- Player sprite: [Source if applicable]
- Ally sprite: 
- Tile assets: [Source if applicable]

## Conclusion

This project successfully implements a complete behavior tree-based AI system with multiple priority levels, complex decision-making, and balanced gameplay. The modular architecture allows for easy expansion and modification, while the current implementation provides a solid foundation for more advanced AI features.

The enemy AI demonstrates believable behavior through its patrol-chase-attack-flee cycle, and the ally summoning mechanic adds strategic depth. The collision system optimizations ensure smooth gameplay without physics issues.

Future improvements could include pathfinding, line-of-sight checks, and more sophisticated group coordination, but the current implementation meets all assignment requirements and provides an engaging gameplay experience.
