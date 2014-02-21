// Code generated by protoc-gen-go.
// source: multi/multi2.proto
// DO NOT EDIT!

package multi

import proto "code.google.com/p/goprotobuf/proto"
import json "encoding/json"
import math "math"

// Reference proto, json, and math imports to suppress error if they are not otherwise used.
var _ = proto.Marshal
var _ = &json.SyntaxError{}
var _ = math.Inf

type Multi2_Color int32

const (
	Multi2_BLUE  Multi2_Color = 1
	Multi2_GREEN Multi2_Color = 2
	Multi2_RED   Multi2_Color = 3
)

var Multi2_Color_name = map[int32]string{
	1: "BLUE",
	2: "GREEN",
	3: "RED",
}
var Multi2_Color_value = map[string]int32{
	"BLUE":  1,
	"GREEN": 2,
	"RED":   3,
}

func (x Multi2_Color) Enum() *Multi2_Color {
	p := new(Multi2_Color)
	*p = x
	return p
}
func (x Multi2_Color) String() string {
	return proto.EnumName(Multi2_Color_name, int32(x))
}
func (x *Multi2_Color) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(Multi2_Color_value, data, "Multi2_Color")
	if err != nil {
		return err
	}
	*x = Multi2_Color(value)
	return nil
}

type Multi2 struct {
	RequiredValue    *int32        `protobuf:"varint,1,req,name=required_value" json:"required_value,omitempty"`
	Color            *Multi2_Color `protobuf:"varint,2,opt,name=color,enum=multi.Multi2_Color" json:"color,omitempty"`
	XXX_unrecognized []byte        `json:"-"`
}

func (m *Multi2) Reset()         { *m = Multi2{} }
func (m *Multi2) String() string { return proto.CompactTextString(m) }
func (*Multi2) ProtoMessage()    {}

func (m *Multi2) GetRequiredValue() int32 {
	if m != nil && m.RequiredValue != nil {
		return *m.RequiredValue
	}
	return 0
}

func (m *Multi2) GetColor() Multi2_Color {
	if m != nil && m.Color != nil {
		return *m.Color
	}
	return Multi2_BLUE
}

func init() {
	proto.RegisterEnum("multi.Multi2_Color", Multi2_Color_name, Multi2_Color_value)
}
