using Content.Shared.Inventory;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Body.Systems;

public abstract partial class SharedBodySystem : EntitySystem
{
    /*
     * See the body partial for how this works.
     */

    /// <summary>
    /// Container ID prefix for any body parts.
    /// </summary>
    public const string PartSlotContainerIdPrefix = "body_part_slot_";

    /// <summary>
    /// Container ID for the ContainerSlot on the body entity itself.
    /// </summary>
    public const string BodyRootContainerId = "body_root_part";

    /// <summary>
    /// Container ID prefix for anybody organs.
    /// </summary>
    public const string OrganSlotContainerIdPrefix = "body_organ_slot_";

    [Dependency] private readonly IRobustRandom _random = default!; // backmen edit
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly InventorySystem _inventorySystem = default!; // backmen edit
    [Dependency] protected readonly IPrototypeManager Prototypes = default!;
    [Dependency] protected readonly MovementSpeedModifierSystem Movement = default!;
    [Dependency] protected readonly SharedContainerSystem Containers = default!;
    [Dependency] protected readonly SharedTransformSystem SharedTransform = default!;
    [Dependency] protected readonly StandingStateSystem Standing = default!;

    public override void Initialize()
    {
        base.Initialize();

        InitializeBody();
        InitializeParts();
        InitializeOrgans();
        // To try and mitigate the server load due to integrity checks, we set up a Job Queue.
        InitializePartAppearances();
    }

    /// <summary>
    /// Inverse of <see cref="GetPartSlotContainerId"/>
    /// </summary>
    public static string GetPartSlotContainerIdFromContainer(string containerSlotId)
    {
        // This is blursed
        var slotIndex = containerSlotId.IndexOf(PartSlotContainerIdPrefix, StringComparison.Ordinal);

        if (slotIndex < 0)
            slotIndex = 0;

        var slotId = containerSlotId.Remove(slotIndex, PartSlotContainerIdPrefix.Length);
        return slotId;
    }

    /// <summary>
    /// Gets the container ID for the specified slotId.
    /// </summary>
    public static string GetPartSlotContainerId(string slotId)
    {
        return PartSlotContainerIdPrefix + slotId;
    }

    /// <summary>
    /// Gets the container ID for the specified slotId.
    /// </summary>
    public static string GetOrganContainerId(string slotId)
    {
        return OrganSlotContainerIdPrefix + slotId;
    }
}
