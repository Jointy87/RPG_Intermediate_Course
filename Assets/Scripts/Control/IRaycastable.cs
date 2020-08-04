namespace RPGCourse.Control
{
	public interface IRaycastable
	{
		bool HandleRaycast(PlayerController callingController);
		CursorType GetCursorType();
	}
}

