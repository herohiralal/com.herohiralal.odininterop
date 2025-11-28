package src

Color32 :: struct {
	rgba: i32,
}

#assert(size_of(quaternion128) == 16)
#assert(align_of(quaternion128) == 4)
