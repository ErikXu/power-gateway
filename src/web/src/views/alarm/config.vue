<template>
  <div class="app-container">
    <h3>{{ $t('Alarm Config') }}</h3>
    <el-row type="flex" style="margin-bottom:10px;" justify="end">
      <el-button size="mini" type="primary" @click="create">{{ $t('Create') }}</el-button>
    </el-row>

    <el-table
      :data="list"
      border
      fit
      highlight-current-row
      tooltip-effect="light"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Name')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Type')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Bot Url')" align="left">
        <template slot-scope="{row}">
          <el-tooltip class="item" effect="light" :content="row.botUrl" placement="top">
            <el-link type="primary">{{ $t('View') }}</el-link>
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Create Time')" align="left" width="160">
        <template slot-scope="{row}">
          <span>{{ row.createAt | simpleFormat }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Operation')" align="center" width="80">
        <template slot-scope="{row}">
          <el-button type="success" size="mini" @click="check(row)">
            {{ $t('Check') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog :title="$t('Alarm Config')" :visible.sync="formVisible">
      <el-form ref="configForm" :model="form" label-position="left" label-width="120px" style="width: 600px;" :rules="rules">
        <el-form-item :label="$t('Name')" prop="name">
          <el-input v-model="form.name" autocomplete="off" />
        </el-form-item>
        <el-form-item :label="$t('Type')" prop="type">
          <el-input v-model="form.type" autocomplete="off" />
        </el-form-item>
        <el-form-item :label="$t('Bot Url')" prop="botUrl">
          <el-input v-model="form.botUrl" autocomplete="off" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="reset">{{ $t('Reset') }}</el-button>
        <el-button type="primary" :disabled="submiting" :loading="submiting" @click.native.prevent="submit">{{ $t('Submit') }}</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>

import { getAlarmConfig, createAlarmConfig, checkAlarmConfig } from '@/api/alarm'

export default {
  data() {
    return {
      list: [],
      formVisible: false,
      form: {
        name: '',
        type: 'lark',
        botUrl: ''
      },
      rules: {
        name: [{ required: true, message: 'Please input name', trigger: 'change' }],
        type: [{ required: true, message: 'Please input type', trigger: 'change' }],
        botUrl: [{ required: true, message: 'Please input alarm config', trigger: 'change' }]
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      getAlarmConfig().then(response => {
        this.list = response
      })
    },
    create() {
      this.submiting = false
      this.formVisible = true
    },
    check(row) {
      checkAlarmConfig(row.id).then(response => {
        this.$message({
          type: 'success',
          message: 'Success!'
        })
      })
    },
    submit() {
      this.$refs.configForm.validate(valid => {
        if (valid) {
          this.submiting = true
          createAlarmConfig(this.form).then(() => {
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
      this.$refs.configForm.resetFields()
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Alarm Config": "Alarm Config",
    "Create": "Create",
    "Name": "Name",
    "Reset": "Reset",
    "Submit": "Submit",
    "Type": "Type",
    "Bot Url": "Bot Url",
    "Create Time": "Create Time",
    "Operation": "Operation",
    "Check": "Check",
    "View": "View"
  },
  "zh": {
    "Alarm Config": "告警配置",
    "Create": "创建",
    "Name": "名称",
    "Reset": "重置",
    "Submit": "提交",
    "Type": "类型",
    "Bot Url": "机器人地址",
    "Create Time": "创建日期",
    "Operation": "操作",
    "Check": "检测",
    "View": "查看"
  }
}
</i18n>
