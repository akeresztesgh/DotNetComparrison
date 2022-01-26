import http from 'k6/http';
import { settings } from './settings.js'
import { enrollmentCount, enrollmentResults, quickResults } from './start.js';

export function quick() {
    let res = http.get(`${settings.baseUrl}/api/school/quick`);
    quickResults.add(res.timings.duration);
}

export function getEnrollmentCount() {
    let res = http.get(`${settings.baseUrl}/api/school/enrollments`);
    enrollmentCount.add(res.timings.duration);
    let count = parseInt(res.body);
    let start = Math.max(Math.floor(Math.random() * count) - 100, 0);
    let end = 100;
    //console.log(`start: ${start}.  count: ${end}`);

    return { start, end };
}

export function getEnrollments(start, count) {
    let res = http.get(`${settings.baseUrl}/api/school/enrollments/${start}/${count}`);
    enrollmentResults.add(res.timings.duration);
}
