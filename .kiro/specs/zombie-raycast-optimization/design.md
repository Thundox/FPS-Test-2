# Design Document

## Overview

This design optimizes the zombie AI's player detection system by implementing a coroutine-based timing mechanism that performs raycast operations at configurable intervals (defaulting to 1 second) instead of every frame. The solution maintains the existing zombie behavior while significantly reducing CPU overhead from expensive raycast operations.

## Architecture

The optimization introduces a centralized raycast timing system within the existing `Zombie` class that:

1. **Replaces frame-based raycast calls** with coroutine-based interval timing
2. **Maintains state consistency** by caching raycast results between intervals
3. **Preserves existing behavior** by ensuring all state transitions remain responsive
4. **Provides configuration flexibility** through inspector-accessible timing parameters

### Key Components Modified

- **IdleBehaviour()**: Remove direct `HasClearPathToPlayer()` call, use cached result
- **WalkingBehaviour()**: Remove direct raycast calls, use cached results for obstacle detection
- **Trigger System**: Standardize `InvokeRepeating` to use configurable interval
- **New Coroutine System**: Centralized raycast timing management

## Components and Interfaces

### New Fields

```csharp
[SerializeField]
private float raycastInterval = 1.0f; // Configurable raycast interval

private bool hasPlayerLineOfSight = false; // Cached raycast result
private bool hasObstacleAhead = false; // Cached obstacle detection result
private Coroutine raycastCoroutine; // Reference to active raycast coroutine
```

### New Methods

```csharp
private IEnumerator RaycastUpdateCoroutine()
// Handles periodic raycast updates for player detection and obstacle checking

private void StartRaycastUpdates()
// Initializes the raycast coroutine when zombie becomes active

private void StopRaycastUpdates()
// Safely stops raycast coroutine to prevent memory leaks

private void UpdateRaycastResults()
// Performs actual raycast operations and updates cached results
```

### Modified Methods

```csharp
private void IdleBehaviour()
// Use cached hasPlayerLineOfSight instead of direct raycast

private void WalkingBehaviour()
// Use cached hasObstacleAhead instead of direct raycast

public void OnTriggerEnter(Collider other)
// Use raycastInterval instead of hardcoded 1f value

private bool HasClearPathToPlayer()
// Maintain for compatibility but mark as internal use only
```

## Data Models

### Raycast State Management

```csharp
private struct RaycastState
{
    public bool hasPlayerLineOfSight;
    public bool hasObstacleAhead;
    public float lastUpdateTime;
}
```

The zombie will maintain cached raycast results that are updated at the specified interval, ensuring consistent behavior between updates while avoiding expensive per-frame calculations.

## Error Handling

### Coroutine Management
- **Null Reference Protection**: Check for null coroutine references before stopping
- **State Transition Safety**: Ensure raycast coroutine is properly managed during state changes
- **Cleanup on Destroy**: Implement `OnDestroy()` to stop active coroutines

### Configuration Validation
- **Invalid Interval Handling**: Default to 1.0f if raycastInterval is set to 0 or negative
- **Runtime Changes**: Allow inspector changes to take effect immediately via property setter

### Performance Safeguards
- **Maximum Raycast Distance**: Maintain existing distance limitations to prevent excessive calculations
- **Layer Mask Validation**: Ensure obstacleLayerMask is properly configured

## Testing Strategy

### Unit Testing Approach
1. **Timing Verification**: Test that raycasts occur at specified intervals
2. **State Consistency**: Verify cached results remain valid between updates
3. **Performance Measurement**: Compare frame time before and after optimization
4. **Behavior Preservation**: Ensure zombie behavior remains functionally identical

### Integration Testing
1. **Multi-Zombie Scenarios**: Test with multiple zombies to verify independent timing
2. **State Transition Testing**: Verify proper coroutine management during state changes
3. **Configuration Testing**: Test various raycast interval values
4. **Memory Leak Testing**: Verify proper cleanup when zombies are destroyed

### Performance Testing
1. **Frame Rate Impact**: Measure FPS improvement with multiple zombies
2. **Raycast Frequency**: Verify actual raycast call reduction
3. **Memory Usage**: Ensure no memory leaks from coroutine management
4. **Responsiveness**: Confirm AI behavior remains acceptably responsive

## Implementation Considerations

### Backward Compatibility
- Maintain existing public interface for `HasClearPathToPlayer()`
- Preserve all existing zombie behavior patterns
- Keep existing debug visualization functionality

### Configuration Flexibility
- Expose `raycastInterval` in inspector for easy tuning
- Allow runtime modification of interval
- Support different intervals for different zombie types

### Performance Optimization
- Use single coroutine per zombie for all raycast operations
- Cache results to avoid redundant calculations
- Implement proper coroutine lifecycle management

### Debug Support
- Maintain existing `Debug.DrawRay` functionality
- Add optional debug logging for raycast timing
- Provide inspector feedback for current raycast state