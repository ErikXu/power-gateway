<template>
  <div class="app-container">
    <h3>{{ $t('Field Projection') }}</h3>
    <el-row type="flex" style="margin-bottom:10px;" justify="end">
      <el-button size="mini" type="primary">{{ $t('Create') }}</el-button>
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
      <el-table-column :label="$t('Key')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('From Key')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.fromKey }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Mapping')" align="left">
        <template slot-scope="{row}">
          <el-tooltip class="item" effect="light" :content="row.mappingText" placement="top">
            <el-link type="primary">{{ $t('View') }}</el-link>
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Create Time')" align="left" width="160">
        <template slot-scope="{row}">
          <span>{{ row.createAt | simpleFormat }}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>

import { getProjectionList } from '@/api/projection'

export default {
  data() {
    return {
      list: []
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      getProjectionList().then(response => {
        this.list = response
      })
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Field Projection": "Field Projection",
    "Create": "Create",
    "Key": "Key",
    "From Key": "From Key",
    "Mapping": "Mapping"
  },
  "zh": {
    "Field Projection": "字段映射",
    "Create": "创建",
    "Key": "字段",
    "From Key": "源字段",
    "Mapping": "映射表"
  }
}
</i18n>
