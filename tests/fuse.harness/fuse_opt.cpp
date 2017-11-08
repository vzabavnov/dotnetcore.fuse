#include <stdio.h>
#include <fuse/fuse_opt.h>

extern "C" {
	long fuse_args_parse(struct fuse_args *args)
	{
		return (long)args->argc;
	}
}