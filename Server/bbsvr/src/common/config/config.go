package config

import (
	"common/consts"
//	"time"
)

var TableUnitExpType = make([]int32, 99)
var TableDevourCostCoin = make([]int32, 99)
var TableUserRankExp = make([]int32, consts.N_MAX_USER_RANK)

func InitConfig() {
	for i := int32(0); i < 99; i++ {
		TableUnitExpType[i] = 100 * ((i+1)*(i+1)/3)
	}

	for i := int32(0); i < 99; i++ {
		TableDevourCostCoin[i] = 100 * (i+1)
	}

	for i := int32(0); i < 500; i++ {
		TableUserRankExp[i] = 100*(i+1)
	}
}
