extends CanvasLayer

@onready var animation_player = $AnimationPlayer
@onready var ui_upgrade_card_0 = $HBox_LevelUpOptions/UIUpgradeCard0
@onready var ui_upgrade_card_1 = $HBox_LevelUpOptions/UIUpgradeCard1
@onready var ui_upgrade_card_2 = $HBox_LevelUpOptions/UIUpgradeCard2

signal quit_level_up
signal add_item
signal add_upgrade

func _process(delta):
	if Input.is_action_just_pressed("card_1"):
		ui_upgrade_card_0.button.button_pressed = true
		_on_button_a_pressed()
	
	if Input.is_action_just_pressed("card_2"):
		ui_upgrade_card_1.button.button_pressed = true
		_on_button_b_pressed()
	
	if Input.is_action_just_pressed("card_3"):
		ui_upgrade_card_2.button.button_pressed = true
		_on_button_c_pressed()

func shuffle_options(options: Array) -> void:
	ui_upgrade_card_0.button.button_pressed = false
	ui_upgrade_card_1.button.button_pressed = false
	ui_upgrade_card_2.button.button_pressed = false
	ui_upgrade_card_0.update_info(options[0])
	ui_upgrade_card_1.update_info(options[1])
	ui_upgrade_card_2.update_info(options[2])

func _on_quit_button_pressed():
	emit_signal("quit_level_up")

func focus():
	ui_upgrade_card_0.grab_focus()

func _on_button_a_pressed():
	emit_signal("add_upgrade", ui_upgrade_card_0.selected)
	emit_signal("quit_level_up")

func _on_button_b_pressed():
	emit_signal("add_upgrade", ui_upgrade_card_1.selected)
	emit_signal("quit_level_up")

func _on_button_c_pressed():
	emit_signal("add_upgrade", ui_upgrade_card_2.selected)
	emit_signal("quit_level_up")
