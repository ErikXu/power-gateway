<template>
  <div class="app-container">
    <h3>{{ $t('Alarm Rule') }}</h3>
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
      <el-table-column :label="$t('Title')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Rule')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.field }} {{ row.operator }} {{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Alarm Config')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.alarmConfigText }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Create Time')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.createAt | simpleFormat }}</span>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog :title="$t('Alarm Rule')" :visible.sync="formVisible">
      <el-form ref="ruleForm" :model="form" label-position="left" label-width="120px" style="width: 600px;" :rules="rules">
        <el-form-item :label="$t('Title')" prop="title">
          <el-input v-model="form.title" autocomplete="off" />
        </el-form-item>
        <el-form-item :label="$t('Field')" prop="field">
          <el-select v-model="form.field" style="width:480px;">
            <el-option
              v-for="field in fieldList"
              :key="field.key"
              :label="field.value"
              :value="field.key"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('Operator')" prop="operator">
          <el-select v-model="form.operator" style="width:480px;">
            <el-option
              v-for="operator in operatorList"
              :key="operator.key"
              :label="operator.value"
              :value="operator.key"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('Value')" prop="value">
          <el-input v-model="form.value" autocomplete="off" />
        </el-form-item>
        <el-form-item :label="$t('Alarm Config')" prop="alarmConfigId">
          <el-select v-model="form.alarmConfigId" style="width:480px;">
            <el-option
              v-for="config in configList"
              :key="config.key"
              :label="config.value"
              :value="config.key"
            />
          </el-select>
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

import { getAlarmRules, getFieldOptions, getOperatorOptions, getConfigOptions, createAlarmRule } from '@/api/alarm'

export default {
  data() {
    return {
      list: [],
      fieldList: [],
      operatorList: [],
      configList: [],
      formVisible: false,
      form: {
        title: '',
        field: '',
        operator: '',
        value: '',
        alarmConfigId: ''
      },
      rules: {
        title: [{ required: true, message: 'Please input title', trigger: 'change' }],
        value: [{ required: true, message: 'Please input value', trigger: 'change' }]
      }
    }
  },
  created() {
    getFieldOptions().then(response => {
      this.fieldList = response
      this.form.field = this.fieldList[0].key
    })

    getOperatorOptions().then(response => {
      this.operatorList = response
      this.form.operator = this.operatorList[0].key
    })

    getConfigOptions().then(response => {
      this.configList = response
    })

    this.fetchData()
  },
  methods: {
    fetchData() {
      getAlarmRules().then(response => {
        this.list = response
      })
    },
    create() {
      this.submiting = false
      this.formVisible = true
    },
    submit() {
      this.$refs.ruleForm.validate(valid => {
        if (valid) {
          this.submiting = true
          createAlarmRule(this.form).then(() => {
            this.$message({
              type: 'success',
              message: 'Submit success!'
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
      this.$refs.ruleForm.resetFields()
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Alarm Rule": "Alarm Rule",
    "Create": "Create",
    "Title": "Title",
    "Field": "Field",
    "Operator": "Operator",
    "Value": "Value",
    "Alarm Config": "Alarm Config",
    "Rule": "Rule",
    "Reset": "Reset",
    "Submit": "Submit",
    "Create Time": "Create Time"
  },
  "zh": {
    "Alarm Rule": "告警规则",
    "Create": "创建",
    "Title": "标题",
    "Field": "字段",
    "Operator": "操作符",
    "Value": "数值",
    "Alarm Config": "告警配置",
    "Rule": "规则",
    "Reset": "重置",
    "Submit": "提交",
    "Create Time": "创建时间"
  }
}
</i18n>
