extends Node

var label_settings: LabelSettings = preload("res://Resources/Menu/label_settings.tres")
const KENNEY_PIXEL_SQUARE = preload("res://Art/UI/Kenney Pixel Square.ttf")
const MINECRAFT_FONT = preload("res://Art/UI/Minecraft.ttf")

func display_number(value: int, position: Vector2, is_critical: bool) -> void:
	var number = Label.new()
	number.global_position = position
	number.text = str(value)
	number.z_index = 5
	#number.label_settings = label_settings
	number.label_settings = LabelSettings.new()
	
	var color = "#FFF"
	if is_critical:
		color = "#B22"
	
	if value == 0:
		color = "#FFF8"
	number.label_settings.font = MINECRAFT_FONT
	number.label_settings.font_color = color
	number.label_settings.font_size = 6
	#number.label_settings.outline_color = "#0000"
	
	call_deferred("add_child", number)
	await number.resized
	number.pivot_offset = Vector2(number.size/2)
	
	await default_movement(number)
	number.queue_free()


func default_movement(number):
	var tween = get_tree().create_tween()
	tween.set_parallel(true)
	tween.tween_property(
		number, "position:y", number.position.y - 24, 0.25
	).set_ease(Tween.EASE_OUT)
	tween.tween_property(
		number, "position:y", number.position.y, 0.25
	).set_ease(Tween.EASE_OUT).set_delay(0.25)
	tween.tween_property(
		number, "scale", Vector2.ZERO, 0.25
	).set_ease(Tween.EASE_IN).set_delay(0.5)
	
	await tween.finished
