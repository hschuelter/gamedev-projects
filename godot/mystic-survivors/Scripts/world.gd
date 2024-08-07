extends Node2D

@onready var player = $Player
@onready var ui_main = $UI_Main
@onready var ui_level_up = $UI_LevelUp
@onready var ui_screen_game_over = $UI_Screen_GameOver
@onready var ui_screen_you_win = $UI_Screen_YouWin
@onready var ui_screen_pause = $UI_Screen_Pause
@onready var enemy_spawner = $EnemySpawner

@onready var bg_music = $MusicPlayer/BG_Music
const OST_VILLAGE   = preload("res://Music/4 - Village.ogg")
const OST_PEACEFUL  = preload("res://Music/5 - Peaceful.ogg")
const OST_END_THEME = preload("res://Music/8 - End Theme.ogg")
var upgrade_options: Array = []

var player_stats: StatsPlayer
var enemy_count: int

const MAX_WAVES = 10

func _ready():
	bg_music.stream = OST_VILLAGE
	bg_music.play()
	
	get_tree().paused = true
	await get_tree().create_timer(1.0).timeout
	get_tree().paused = false
	
	player.connect("level_up", _on_level_up)
	player.connect("game_over", _on_game_over)
	player_stats = player.base_stats
	
	add_available_upgrades()
	
	ui_main.update_ui(player_stats)
	ui_main.show()
	ui_level_up.hide()
	ui_screen_game_over.hide()
	ui_screen_pause.hide()
	ui_screen_pause.connect("unpause", unpause_game)
	
	ui_level_up.connect("quit_level_up", _on_quit_level_up)
	ui_level_up.connect("add_item", _on_add_item)
	ui_level_up.connect("add_upgrade", _on_add_upgrade)
	enemy_count = 0
	
	enemy_spawner.connect("wave_change", _on_wave_change)
	enemy_spawner.connect("enemy_killed", _on_enemy_killed)
	

func _process(delta):
	if Input.is_action_just_pressed("ui_cancel"):
		if not get_tree().paused:
			ui_screen_pause.update_screen(player, ui_main)
			get_tree().paused = not get_tree().paused
			ui_screen_pause.show()

func unpause_game():
	get_tree().paused = not get_tree().paused
	ui_screen_pause.hide()

func _on_player_update_level_ui(value: int) -> void:
	ui_main.update_level(value)

func _on_wave_change(value: int) -> void:
	ui_main.update_wave_ui(value)
	
	if (value >= MAX_WAVES):
		ui_screen_you_win.update_screen(player, ui_main)
		ui_screen_you_win.show()
		
		bg_music.stream = OST_END_THEME
		bg_music.play()
		
		get_tree().paused = true

func _on_enemy_killed(value: int) -> void:
	ui_main.update_kill_count(value)

func _on_level_up() -> void:
	upgrade_options.shuffle()
	ui_level_up.focus()
	var available_options = []
	
	var i = 0
	while available_options.size() < 3:
		if upgrade_options[i].get_upgrade():
			available_options.push_back(upgrade_options[i])
		i += 1
	
	ui_level_up.shuffle_options(available_options)
	
	get_tree().paused = true
	#ui_main.hide()
	ui_level_up.show()
	ui_level_up.animation_player.play("enter")

func _on_quit_level_up() -> void:
	get_tree().paused = false
	ui_level_up.hide()
	ui_main.show()

func _on_add_item(item: PassiveItem) -> void:
	player.add_item(item)

func _on_add_upgrade(upgrade: Upgrade) -> void:
	if upgrade is UpgradeItem:
		player.add_item(upgrade.passive_item)
	if upgrade is UpgradeWeapon:
		player.add_weapon(upgrade.weapon_data, upgrade.weapon_scene)
		upgrade.level_up()

func _on_game_over():
	ui_screen_game_over.show()
	bg_music.stream = OST_PEACEFUL
	bg_music.play()
	get_tree().paused = true

func add_available_upgrades():
	var bow_data = preload("res://Resources/Weapons/Bow/bow_lv_1.tres")
	var bow_scene = preload("res://Scenes/Skills/skill_bow.tscn")
	var shuriken_data = preload("res://Resources/Weapons/Shuriken/shuriken_lv_0.tres")
	var shuriken_scene = preload("res://Scenes/Skills/skill_shuriken.tscn")
	var fireball_data = preload("res://Resources/Weapons/Fireball/fireball_lv_0.tres")
	var fireball_scene = preload("res://Scenes/Skills/skill_fireball.tscn")
	var lightning_data = preload("res://Resources/Weapons/Lightning/lightning_lv_0.tres")
	var lightning_scene = preload("res://Scenes/Skills/skill_lightning.tscn")
	
	upgrade_options = [
		UpgradeItem.new(CdrUpPassiveItem.new()),
		UpgradeItem.new(AmoUpPassiveItem.new()),
		UpgradeItem.new(DmgUpPassiveItem.new()),
		UpgradeItem.new(DurUpPassiveItem.new()),
		UpgradeItem.new( HpUpPassiveItem.new()),
		UpgradeItem.new(KnoUpPassiveItem.new()),
		UpgradeItem.new(MagUpPassiveItem.new()),
		UpgradeItem.new(MovUpPassiveItem.new()),
		UpgradeItem.new(PieUpPassiveItem.new()),
		UpgradeItem.new(PrsUpPassiveItem.new()),
		UpgradeItem.new(RegUpPassiveItem.new()),
		UpgradeItem.new(SizUpPassiveItem.new()),

		UpgradeWeapon.new(bow_data, bow_scene),
		UpgradeWeapon.new(shuriken_data, shuriken_scene),
		UpgradeWeapon.new(fireball_data, fireball_scene),
		UpgradeWeapon.new(lightning_data, lightning_scene)
	]
