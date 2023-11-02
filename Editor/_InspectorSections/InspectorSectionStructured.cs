public abstract class InspectorSectionStructured : IInspectorSection
{
	public InspectorSectionStructured (IInspectorSection nextSection = null)
	{
		if (nextSection == null)
			NextSection = new InspectorSectionNull ();
		else
			NextSection = nextSection;

		Styles = new PresetStyles ();
	}

	protected IInspectorSection NextSection { get; private set; }

	protected PresetStyles Styles { get; private set; }

	public virtual void Insert ()
	{
		DrawHeader ();
		DrawBody ();
		DrawFooter ();

		NextSection.Insert ();
	}

	protected abstract void DrawHeader ();

	protected abstract void DrawBody ();

	protected virtual void DrawFooter ()
	{
		CustomLayout.DrawSectionLine ();
	}
}
