extends UI_Screen
class_name UI_Screen_Pause

signal unpause

func _on_continue_pressed():
	emit_signal("unpause")
