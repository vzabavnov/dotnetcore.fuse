namespace Fuse.Interop
{
    /// <summary>
    /// Readdir flags, passed to ->readdir()
    /// </summary>
    public enum FuseFillDirFlags
    {
	    // "Plus" mode: all file attributes are valid
	    //
	    // The attributes are used by the kernel to prefill the inode cache
	    // during a readdir.
	    //
	    // It is okay to set FUSE_FILL_DIR_PLUS if FUSE_READDIR_PLUS is not set
	    // and vice versa.
	    FUSE_FILL_DIR_PLUS = 1 << 1,
    }
}
