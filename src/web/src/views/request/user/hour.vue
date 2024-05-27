<template>
  <div class="app-container">
    <h3>{{ $t('User Request 1 Hour') }}</h3>
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
      <el-table-column :label="$t('Time')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.time | simpleFormat }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('UserId')" align="center">
        <template slot-scope="{row}">
          <span>{{ row.userId }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Count')" align="center">
        <template slot-scope="{row}">
          <span>{{ row.count }}</span>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination-container" style="margin-top:10px; text-align:right;">
      <el-pagination background layout="total, sizes, prev, pager, next, jumper" :page-sizes="[10,15,20,30,50]" :current-page="query.pageIndex" :page-size="query.pageSize" :total="query.total" @current-change="pageIndexChange" @size-change="pageSizeChange" />
    </div>
  </div>
</template>

<script>
import { getUserRequest1Hour } from '@/api/request'

export default {
  data() {
    return {
      list: [],
      query: {
        pageIndex: 1,
        pageSize: 10,
        total: 0
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      getUserRequest1Hour(this.query.pageIndex, this.query.pageSize).then(response => {
        this.list = response.records
        this.query.total = response.total
      })
    },
    pageIndexChange(val) {
      this.query.pageIndex = val
      this.fetchData()
    },
    pageSizeChange(val) {
      this.query.pageSize = val
      this.fetchData()
    }
  }
}
</script>

<i18n>
{
  "en": {
    "User Request 1 Hour": "User Request 1 Hour",
    "UserId": "UserId",
    "Time": "Time",
    "Count": "Count"
  },
  "zh": {
    "User Request 1 Hour": "每小时用户请求",
    "UserId": "用户标识",
    "Time": "时间",
    "Count": "次数"
  }
}
</i18n>
