extends Upgrade
class_name UpgradeWeapon

func _init(data: WeaponData, scene: Resource):
	self.weapon_data = data
	self.weapon_scene = scene
	self.title = weapon_data.title
	self.level = weapon_data.level
	self.upgrade = weapon_data
	self.icon = Sprite2D.new()
	self.icon.texture = upgrade.icon

func get_upgrade() -> WeaponData:
	return self.upgrade

func is_next() -> bool:
	return self.weapon_data.next != null

func get_level() -> int:
	if get_upgrade():
		return get_upgrade().level
	return -1

func get_description() -> String:
	if get_upgrade():
		return get_upgrade().description
	return "???"

func level_up():
	self.upgrade = self.upgrade.next
	self.level += 1
