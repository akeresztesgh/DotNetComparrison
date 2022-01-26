import { sleep } from 'k6';
import { getEnrollments, getEnrollmentCount, quick } from './api-functions.js';
import { Trend } from 'k6/metrics';

export let quickResults = new Trend('quick');
export let enrollmentCount = new Trend('getEnrollmentCount');
export let enrollmentResults = new Trend('getEnrollments');

//How many users and for how long should run this script
export let options = {
    vus: 10,           // How many VUs (virtual users)
    duration: '60s',  // How long does the test run
};


export default function () {

    console.log(`Virtual User ${__VU}:`);
    quick();
    sleep(1);
    let range = getEnrollmentCount();
    sleep(1);
    getEnrollments(range.start, range.end);
}
