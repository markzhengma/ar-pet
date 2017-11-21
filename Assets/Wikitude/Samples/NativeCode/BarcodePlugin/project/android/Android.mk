LOCAL_PATH := $(call my-dir)
LOCAL_PATH_SAVE := $(LOCAL_PATH)
SRC_DIR := $(LOCAL_PATH)/../../src
LIB_DIR := $(LOCAL_PATH)/../../lib/android
INCLUDE_DIR := $(LOCAL_PATH)/../../src/zbar

LOCAL_PATH := $(LIB_DIR)/$(TARGET_ARCH_ABI)
include $(CLEAR_VARS)
LOCAL_MODULE := iconv
LOCAL_SRC_FILES := libiconv.a
include $(PREBUILT_STATIC_LIBRARY)


LOCAL_PATH := $(LIB_DIR)/$(TARGET_ARCH_ABI)
include $(CLEAR_VARS)
LOCAL_MODULE := zbar
LOCAL_SRC_FILES := libzbar.a
include $(PREBUILT_STATIC_LIBRARY)


LOCAL_PATH      := $(LOCAL_PATH_SAVE)
include $(CLEAR_VARS)
LOCAL_ARM_MODE  := arm
LOCAL_MODULE    := libbarcode
LOCAL_SRC_FILES := $(SRC_DIR)/barcode.cpp
LOCAL_C_INCLUDES := $(INCLUDE_DIR)
LOCAL_STATIC_LIBRARIES += zbar iconv
LOCAL_LDLIBS    := -llog -latomic
include $(BUILD_SHARED_LIBRARY)
