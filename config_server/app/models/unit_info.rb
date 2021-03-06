#encoding: utf-8
require 'zip'
module EUnitType
  UALL = 0
  UFIRE = 1
  UWATER = 2
  UWIND = 3
  ULIGHT = 4
  UDARK = 5
  UNONE = 6
  UHeart = 7
end

module EUnitRace
  ALL = 0
  HUMAN = 1
  UNDEAD = 2
  MYTHIC = 3
  BEAST = 4
  MONSTER = 5
  LEGEND = 6
  DRAGON = 7
  SCREAMCHEESE = 8
  EVOLVEPARTS = 9
end

module EUnitGetType 
  E_NONE				= 0
  E_QUEST				= 1
  E_GACHA_NORMAL		= 2
  E_GACHA_EVENT		= 3
  E_BUY				= 4
end

class UnitInfo
  include Beefcake::Message
end

class PowerType
  include Beefcake::Message
end

class HelperRequire
  include Beefcake::Message
end

class EvolveInfo
  include Beefcake::Message
end

class UnitGetWay
  include Beefcake::Message
end

class PowerInfo
  include Beefcake::Message
end

class UnitGetWay
  optional :getType, EUnitGetType, 1
  repeated :getPath, :uint32, 2
end


class UnitInfo
  UNITTYPE = { "UALL" => 0 , "UFIRE" => 1, "UWATER" => 2, "UWIND" => 3, "ULIGHT" => 4 ,"UDARK" => 5,"UNONE" => 6,"UHeart" => 7 }
  UNITRACE = { "ALL" => 0 , "HUMAN" => 1, "UNDEAD" =>2 , "MYTHIC" => 3 , "BEAST" => 4, "MONSTER" => 5 ,"LEGEND" => 6, "DRAGON" => 7,"SCREAMCHEESE" => 8, "EVOLVEPARTS" => 9 }
  EUNIT_GETWAY = { "E_NONE" => 0,"E_QUEST" => 1 ,"E_GACHA_NORMAL" => 2 ,"E_GACHA_EVENT" => 3,"E_BUY" => 4}  
  
  required :id, :uint32, 1
  optional :name, :string, 2
  optional :race, EUnitRace, 3
  optional :type, EUnitType, 4
  optional :rare, :int32, 5
  optional :skill1, :int32, 6
  optional :skill2, :int32, 7
  optional :leaderSkill, :int32, 8
  optional :activeSkill, :int32, 9
  optional :passiveSkill, :int32, 10
  #optional :activeSkillLevel,:int32, 11;
  optional :maxLevel, :int32, 11
  optional :profile, :string, 12
  optional :powerType, PowerType, 13
  optional :evolveInfo, EvolveInfo, 14
  optional :cost, :int32, 15
  optional :saleValue, :int32, 16
  optional :devourValue, :int32, 17
  optional :getWay,  UnitGetWay, 18
  optional :maxActiveSkillLv,  :int32, 19
  optional :maxStar,  :int32, 20
  
  def self.create_with_params(params)
    power_type = PowerType.new(attackType:  params_to_i(params[:attackType]),hpType: params_to_i(params[:hpType]),expType: params_to_i(params[:expType]))
    hepler_require = HelperRequire.new(level: params_to_i(params[:level]),race: params_to_i(params[:helper_race]),type: params_to_i(params[:helper_type]))
    materialUnitId = build_materialUnitId(params[:materialUnitId1],params[:materialUnitId2],params[:materialUnitId3])
    envolve_info = EvolveInfo.new(evolveUnitId: params_to_i(params[:evolveUnitId]) ,materialUnitId: materialUnitId,helperRequire: hepler_require,evolveQuestId: params_to_i(params[:evolveQuestId]))
    unit_info =  UnitInfo.new(
    id: params_to_i(params[:id]),
    name: params[:name],
    race: params_to_i(params[:race]),
    type: params_to_i(params[:type]),
    rare: params_to_i(params[:rare]),
    skill1: params_to_i(params[:skill1]),
    skill2: params_to_i(params[:skill2]),
    leaderSkill: params_to_i(params[:leaderSkill]),
    activeSkill: params_to_i(params[:activeSkill]),
    passiveSkill: params_to_i(params[:passiveSkill]),  
    #activeSkillLevel: params_to_i(params[:activeSkillLevel]),
    maxLevel: params_to_i(params[:maxLevel]),
    profile: params[:profile],
    powerType: power_type,
    evolveInfo: envolve_info,
    cost: params_to_i(params[:cost]),
    saleValue: params_to_i(params[:saleValue]),
    devourValue: params_to_i(params[:devourValue]),
    #getWay: params_to_i(params[:getWay])
    maxActiveSkillLv: params_to_i(params[:maxActiveSkillLv]),
    maxStar: params_to_i(params[:maxStar])
    )
  end
  
  def self.build_materialUnitId(p1,p2,p3)
    materialUnitId = []
    materialUnitId << p1.to_i if p1 != "请选择卡牌信息"
    materialUnitId << p2.to_i if p2 != "请选择卡牌信息"
    materialUnitId << p3.to_i if p3 != "请选择卡牌信息"
    materialUnitId.uniq
  end
  
  def self.to_zip
    FileUtils.rm_rf(Rails.root.join("public/unit/."))
    redis_to_file
    directory = Rails.root.join("public/unit")
    zipfile_name = Rails.root.join("public/unit/units.zip")
    File.delete(zipfile_name) if File.exist?(zipfile_name)
    
    Zip::File.open(zipfile_name, Zip::File::CREATE) do |zipfile|
      Dir[File.join(directory, '**', '**')].each do |file|
        zipfile.add(File.basename(file),file )
      end
    end
  end
  
  def self.params_to_i(s)
    (s == "") ? nil : s.to_i
  end
  
  def self.redis_to_file
    $redis.keys.map{|k|k if k.start_with?("X_UNIT_")}.compact.each do |key|
      File.open(Rails.root.join("public/unit/#{key.split("_")[2]}.bytes"), "wb") { | file|  file.write($redis.get key) } 
    end
  end
  
  def save_to_file
    File.open(Rails.root.join("public/unit/#{self["id"]}.bytes"), "wb") { | file|  file.write(self.encode) } 
  end
  
  def save_to_redis
    $redis.set "X_UNIT_"+self["id"].to_s,self.encode
  end
end

class PowerInfo
  optional :min, :int32, 1
  optional :max, :int32, 2
  optional :growCurve, :float, 3
end

class PowerType
  optional :attackType, PowerInfo, 1
  optional :hpType, PowerInfo, 2
  optional :expType, PowerInfo, 3
end



class HelperRequire
  optional :level, :int32, 1
  optional :race, EUnitRace, 2
  optional :type, EUnitType, 3
end

class EvolveInfo
  required :evolveUnitId, :uint32, 1
  repeated :materialUnitId, :uint32, 2
  optional :helperRequire, HelperRequire, 3
  optional :evolveQuestId, :uint32, 4
end
