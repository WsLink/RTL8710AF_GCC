extern "C" {
	#include "diag.h"
	#include "gpio_api.h"   // mbed
}

int main(void)
{
    gpio_t gpio_led;

	DiagPrintf("万家灯火，无声物联！\r\n");

    // Init LED control pin
    gpio_init(&gpio_led, PC_5);
    gpio_dir(&gpio_led, PIN_OUTPUT);    // Direction: Output
    gpio_mode(&gpio_led, PullNone);     // No pull
    while(1){
        gpio_write(&gpio_led, !gpio_read(&gpio_led));
        //RtlMsleepOS(1000);
		volatile int n = 100000;
		while(n--);
    }

	return 0;
}
