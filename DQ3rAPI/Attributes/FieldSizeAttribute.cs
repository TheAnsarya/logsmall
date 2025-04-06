namespace DQ3rAPI.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
class FieldSizeAttribute : Attribute {
	// The constructor is called when the attribute is set.
	public FieldSizeAttribute(Ima size) {
		_size = size;
	}

	protected Ima _size;

	public Ima Size {
		get { return _size; }
		set { _size = value; }
	}
}
