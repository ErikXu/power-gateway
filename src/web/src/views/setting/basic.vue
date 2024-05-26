<template>
  <div class="app-container">
    <h3>{{ $t('Basic Setting') }}</h3>

    <el-form ref="form" :model="form" label-width="140px">
      <el-form-item :label="$t('Latency Benchmark')" prop="latency">
        <el-input v-model="form.latency" />
      </el-form-item>
      <el-form-item :label="$t('UserId Filed')" prop="userIdField">
        <el-input v-model="form.userIdField" />
      </el-form-item>
      <el-form-item>
        <el-button @click="reset">{{ $t('Reset') }}</el-button>
        <el-button type="primary" :disabled="submiting" :loading="submiting" @click.native.prevent="submit">{{ $t('Save') }}</el-button>
      </el-form-item>
    </el-form>

  </div>
</template>

<script>

import { getBasicSetting, saveBasicSetting } from '@/api/setting'

export default {
  data() {
    return {
      submiting: false,
      form: {
        latency: 500,
        userIdField: 'userId'
      },
      rules: {
        latency: [{ required: true, message: 'Please input latency', trigger: 'change' }],
        userIdField: [{ required: true, message: 'Please input userId field', trigger: 'change' }]
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      getBasicSetting().then(response => {
        this.form = response
      })
    },
    submit() {
      this.$refs.form.validate(valid => {
        if (valid) {
          this.submiting = true
          saveBasicSetting(this.form).then(() => {
            this.$message({
              type: 'success',
              message: 'Success!'
            })
            this.fetchData()
            this.formVisible = false
            this.submiting = false
          })
        } else {
          return false
        }
      })
    },
    reset() {
      this.$refs.form.resetFields()
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Basic Setting": "Basic Setting",
    "Latency Benchmark": "Latency Benchmark",
    "UserId Filed": "UserId Filed",
    "Save": "Save",
    "Reset": "Reset"
  },
  "zh": {
    "Basic Setting": "基础设置",
    "Latency Benchmark": "延迟基准",
    "UserId Filed": "用户标识",
    "Save": "保存",
    "Reset": "重置"
  }
}
</i18n>
