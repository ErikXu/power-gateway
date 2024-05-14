<template>
  <div class="app-container">
    <h3>{{ $t('Request Log') }}</h3>
    <el-table
      :data="list"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Url')" align="left">
        <template slot-scope="{row}">
          <el-tag v-if="row.request.method === 'GET'" size="small" effect="dark" color="#61AFFE" style="border-color: #d9ecff;">{{ row.request.method }}</el-tag>
          <el-tag v-if="row.request.method === 'POST'" size="small" effect="dark" color="#49C990" style="border-color: #d9ecff;">{{ row.request.method }}</el-tag>
          <el-tag v-if="row.request.method === 'PUT'" size="small" effect="dark" color="#FCA130" style="border-color: #d9ecff;">{{ row.request.method }}</el-tag>
          <el-tag v-if="row.request.method === 'PATCH'" size="small" effect="dark" color="#50E3C2" style="border-color: #d9ecff;">{{ row.request.method }}</el-tag>
          <el-tag v-if="row.request.method === 'DELETE'" size="small" effect="dark" color="#F93E3E" style="border-color: #d9ecff;">{{ row.request.method }}</el-tag>
          <span> {{ row.request.url }} </span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Upstream')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.upstream }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Latency')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.latency }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Status')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.response.status }}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { getApisixRequestLogList } from '@/api/log'

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
      getApisixRequestLogList().then(response => {
        this.list = response
      })
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Request Log": "Request Log",
    "Url": "Url",
    "Upstream": "Upstream",
    "Latency": "Latency - ms",
    "Status": "Status"
  },
  "zh": {
    "Request Log": "请求日志",
    "Url": "Url",
    "Upstream": "Upstream",
    "Latency": "延时 - 毫秒",
    "Status": "状态码"
  }
}
</i18n>
