namespace VprModLib.Serialization
{
    public static class VprModelUtility
    {
        public static bool TryBuildModel(SerializedSequence serializedSequence, out Sequence? sequence, out string message)
        {
			sequence = null;
            message = $"Unhandled exception in TryBuildModel().";

            try
			{
				sequence = serializedSequence.ToModel();
				message = "Success.";
				return true;
			}
			catch (Exception e)
			{
				message = "There was a problem building the runtime model for the serialized sequence: " + e;
			}

			return false;
        }
		public static bool TryBuildSerialized(Sequence sequence, out SerializedSequence? serializedSequence, out string message)
		{
			serializedSequence = null;
            message = $"Unhandled exception in TryBuildSerialized().";

            try
			{
				serializedSequence = new SerializedSequence(sequence);
				message = "Success.";
				return true;
			}
			catch (Exception e)
			{
				message = "There was a problem building the serialized sequence from the runtime model: " + e;
			}
            return false;
        }
    }
}