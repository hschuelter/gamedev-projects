extends ColorRect

@onready var back_sprite = $BackSprite
@onready var front_sprite = $FrontSprite
@onready var animation_player = $AnimationPlayer

func play_animation() -> void:
	self.show()
	animation_player.play("back_flip")
