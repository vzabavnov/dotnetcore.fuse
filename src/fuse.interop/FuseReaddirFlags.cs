namespace Fuse.Interop
{
    /// <summary>
    /// Readdir flags, passed to ->readdir()
    /// </summary>
    public enum FuseReaddirFlags
    {
        // "Plus" mode.
        //
	    // The kernel wants to prefill the inode cache during readdir.  The
	    // filesystem may honour this by filling in the attributes and setting
	    // FUSE_FILL_DIR_FLAGS for the filler function.  The filesystem may also
	    // just ignore this flag completely.
	    FUSE_READDIR_PLUS = 1 << 0
    }
}
