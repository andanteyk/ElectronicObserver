namespace ElectronicObserverUpdater
{
	/// <summary>
	/// This indicates extracting progress of zip file.
	/// </summary>
	public class ZipProgress
	{
		/// <summary>
		/// Init ZipProgress object
		/// </summary>
		/// <param name="total">default total value</param>
		/// <param name="processed">default progress value</param>
		/// <param name="currentItem">currently working item</param>
		public ZipProgress(int total, int processed, string currentItem)
		{
			Total = total;
			Processed = processed;
			CurrentItem = currentItem;
		}
		/// <summary>
		/// Total item count of zip file.
		/// </summary>
		public int Total { get; }
		/// <summary>
		/// Extracted item count of zip file.
		/// </summary>
		public int Processed { get; }
		/// <summary>
		/// The filename of currently extracting.
		/// </summary>
		public string CurrentItem { get; }
	}

}