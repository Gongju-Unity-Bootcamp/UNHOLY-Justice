using CleverCrow.Fluid.BTs.Trees;

public static class BehaviorTreeBuilderExtensions
{
    public static BehaviorTreeBuilder CustomAction(this BehaviorTreeBuilder builder, string statename = "Ani Name")
    {
        return builder.AddNode(new CustomAction(statename)
        {
            Name = statename,
        });
    }
}